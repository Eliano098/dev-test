using Application.Common.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Client.Services
{
    public class ClientImportProcessor : IClientImportProcessor
    {
        private readonly IClientControlContext _context;
        private readonly IClientImportFileStorage _fileStorage;

        public ClientImportProcessor(IClientControlContext context, IClientImportFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task ProcessPendingAsync(CancellationToken cancellationToken)
        {
            var imports = await _context.ClientImports
                .Where(i => i.Status == ClientImportStatus.Pending || i.Status == ClientImportStatus.Processing)
                .ToListAsync(cancellationToken);

            foreach (var clientImport in imports)
            {
                try
                {
                    clientImport.Start();
                    await _context.SaveChangesAsync(cancellationToken);

                    var existingDocuments = new HashSet<string>(
                        await _context.Clients.AsNoTracking()
                            .Select(client => client.DocumentNumber)
                            .ToListAsync(cancellationToken),
                        StringComparer.OrdinalIgnoreCase);
                    var importedDocuments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var clientsToImport = new List<Domain.Client>();

                    using var content = _fileStorage.OpenRead(clientImport.FilePath);
                    using var reader = new StreamReader(content);
                    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        PrepareHeaderForMatch = args => args.Header.Trim()
                    });

                    if (await csv.ReadAsync())
                    {
                        csv.ReadHeader();

                        while (await csv.ReadAsync())
                        {
                            try
                            {
                                var firstName = csv.GetField("FirstName");
                                var lastName = csv.GetField("LastName");
                                var phoneNumber = csv.GetField("PhoneNumber");
                                var email = csv.GetField("Email");
                                var documentNumber = csv.GetField("DocumentNumber");
                                var birthDateValue = csv.GetField("BirthDate");
                                var postalCode = csv.GetField("PostalCode");
                                var addressLine = csv.GetField("AddressLine");
                                var number = csv.GetField("Number");
                                var complement = csv.GetField("Complement");
                                var neighborhood = csv.GetField("Neighborhood");
                                var city = csv.GetField("City");
                                var state = csv.GetField("State");

                                if (new[] { firstName, lastName, phoneNumber, email, documentNumber, postalCode, addressLine, number, neighborhood, city, state }
                                    .Any(string.IsNullOrWhiteSpace))
                                    throw new InvalidDataException("Campo obrigatório não informado");

                                if (!DateTime.TryParseExact(birthDateValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate)
                                    || birthDate < new DateTime(1000, 1, 1))
                                    throw new InvalidDataException("Data de nascimento inválida");

                                if (!importedDocuments.Add(documentNumber) || existingDocuments.Contains(documentNumber))
                                    throw new InvalidDataException("Documento já cadastrado");

                                clientsToImport.Add(new Domain.Client(
                                    firstName,
                                    lastName,
                                    phoneNumber,
                                    email,
                                    documentNumber,
                                    birthDate,
                                    new Domain.Address(postalCode, addressLine, number, complement, neighborhood, city, state)));
                                clientImport.RegisterSuccess();
                            }
                            catch
                            {
                                clientImport.RegisterFailure();
                            }
                        }
                    }

                    await _context.Clients.AddRangeAsync(clientsToImport, cancellationToken);
                    clientImport.Complete();
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    clientImport.Fail(exception.Message);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
