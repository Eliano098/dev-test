import React, { Suspense, useState } from "react";
import { Button, Card, Form, Row } from "react-bootstrap";
import { NAVIGATION_PATH } from "@/constants";
import { Client } from "@/types/api/Client";
import DataTable from "@/components/DataTable";
import { ActionItemType, CrudActions } from "@/components/CrudActions";
import { Link, useNavigate } from "react-router-dom";
import { mountRoute } from "@/utils/mountRoute";
import Loader from "@/components/Loader";
import ClientService from "@/services/ClientService";
import { TextFormFieldType } from "@/components/form/TextFormField/TextFormFieldType";
import { ClientFilter } from "@/types/api/filters/ClientFilter";
import CustomModal from "@/components/CustomModal";
import { toastr } from "@/utils/toastr";

const ClientListing = () => {
    const navigate = useNavigate();
    const [isImportModalOpen, setIsImportModalOpen] = useState(false);
    const [importFile, setImportFile] = useState<File>();
    const [isImporting, setIsImporting] = useState(false);

    function closeImportModal() {
        if (!isImporting) {
            setIsImportModalOpen(false);
            setImportFile(undefined);
        }
    }

    function selectImportFile(file?: File) {
        if (file && !file.name.toLowerCase().endsWith(".csv")) {
            setImportFile(undefined);
            toastr({ title: "Arquivo inválido", text: "Selecione um arquivo CSV.", icon: "error" });
            return;
        }

        setImportFile(file);
    }

    async function importClients() {
        if (!importFile) return;

        try {
            setIsImporting(true);
            await ClientService.importCsv(importFile);
            toastr({ title: "Arquivo recebido", text: "A importação está sendo processada e os novos clientes aparecerão na listagem em breve.", icon: "success" });
            setImportFile(undefined);
            setIsImportModalOpen(false);
        } catch (err: any) {
            toastr({ title: "Erro ao importar clientes", text: err.message, icon: "error" });
        } finally {
            setIsImporting(false);
        }
    }

    return <>
        <div style={{ display: "flex", justifyContent: "flex-end", alignItems: "center", gap: "8px", margin: "10px 0" }}>
            <Link to={NAVIGATION_PATH.CLIENTS.CREATE.ABSOLUTE}>
                <Button style={{ maxWidth: "fit-content", float: "right" }}>Adicionar</Button>
            </Link>
            <Button variant="outline-primary" style={{ maxWidth: "fit-content" }} onClick={() => setIsImportModalOpen(true)}>
                Importar CSV
            </Button>
        </div>
        <Card >
            <Card.Title></Card.Title>
            <Card.Header>
                <Card.Title>
                    Clientes
                </Card.Title>
            </Card.Header>
            <Suspense fallback={<><Loader /><br /><br /></>}>
                <DataTable<Client, ClientFilter>
                    thin
                    columns={[
                        
                        { Header: "Nome", accessor: "firstName" },
                        { Header: "Sobrenome", accessor: "lastName" },
                        { Header: "Email", accessor: "email" },
                        { Header: "Telefone", accessor: "phoneNumber" },
                        { Header: "Documento", accessor: "documentNumber" },
                        {
                            Header: "Data de nascimento",
                            accessor: "birthDate",
                            Cell: ({ value }) => value.split("T")[0].split("-").reverse().join("/"),
                        },
                        {
                            Header: "Ações",
                            id: "actions",
                            Cell: ({ row }) => (
                                <CrudActions
                                    cell={row.original}
                                    actions={[
                                        {
                                            type: ActionItemType.EDIT,
                                            tooltipLabel: "Editar",
                                            handler: (client) => {
                                                if (client.id) {
                                                    navigate(mountRoute(NAVIGATION_PATH.CLIENTS.EDIT.ABSOLUTE, { id: client.id }));
                                                }
                                            },
                                        },
                                    ]}
                                />
                            ),
                        },
                    ]}
                    query={async (filters) => {
                        const document = filters.find(filter => filter.name === "document")?.value as string | undefined;
                        return await ClientService.getAll(document);
                    }}
                    fetchButton
                    queryOnFilterChange={false}
                    cleanButton
                    filters={[
                        {
                            componentType: TextFormFieldType.INPUT,
                            name: "document",
                            label: "Documento",
                            placeholder: "Documento",
                        },
                    ]}
                    queryName={["client", "listing"]}
                />
            </Suspense>
        </Card >
        <CustomModal
            show={isImportModalOpen}
            onHide={closeImportModal}
            header={{ title: "Importar clientes" }}
            footer={{
                actions: [
                    { label: "Cancelar", variant: "secondary", handler: closeImportModal, disabled: isImporting },
                    { label: isImporting ? "Enviando..." : "Enviar", variant: "primary", handler: importClients, disabled: !importFile || isImporting },
                ],
            }}
        >
            <Form.Group controlId="client-import-file">
                <Form.Label>Arquivo CSV</Form.Label>
                <Form.Control
                    type="file"
                    accept=".csv,text/csv"
                    disabled={isImporting}
                    onChange={(event) => selectImportFile(event.target.files?.[0])}
                />
                <Form.Text>Selecione um arquivo CSV com os dados dos clientes.</Form.Text>
            </Form.Group>
        </CustomModal>
    </>
}

export default ClientListing;
