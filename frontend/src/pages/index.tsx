import React, { Suspense } from "react";
import { Helmet } from "react-helmet-async";
import { Card, Col, ProgressBar, Row } from "react-bootstrap";
import { useSuspenseQuery } from "@tanstack/react-query";

import Loader from "@/components/Loader";
import { ReactQueryKeys } from "@/constants/ReactQueryKeys";
import ClientService from "@/services/ClientService";
import { ClientDashboard } from "@/types/api/ClientDashboard";

const DashboardCards = () => {
  const { data } = useSuspenseQuery<ClientDashboard>({
    queryKey: [ReactQueryKeys.CLIENT, "dashboard"],
    meta: { fetchFn: () => ClientService.getDashboard() },
  });

  const cards = [
    { label: "Total de clientes", value: data.totalClients },
    { label: "Novos nos últimos 30 dias", value: data.newClientsLast30Days },
    { label: "Média de idade", value: `${data.averageAge} anos` },
  ];

  return <>
    <Row className="g-3">
      {cards.map((card) => (
          <Col key={card.label} sm={6} xl={4}>
            <Card className="h-100">
              <Card.Body>
                <p className="text-muted mb-2">{card.label}</p>
                <h2 className="mb-0">{card.value}</h2>
              </Card.Body>
            </Card>
          </Col>
        ))}
    </Row>

    <Card className="mt-4">
      <Card.Header>
        <Card.Title className="mb-0">Distribuição por faixa etária</Card.Title>
      </Card.Header>
      <Card.Body>
        {data.ageRanges.map((ageRange) => {
          const percentage = data.totalClients === 0 ? 0 : Math.round((ageRange.count / data.totalClients) * 100);

          return <div className="mb-3" key={ageRange.name}>
            <div className="d-flex justify-content-between mb-1">
              <span>{ageRange.name}</span>
              <span className="text-muted">{ageRange.count} ({percentage}%)</span>
            </div>
            <ProgressBar now={percentage} aria-label={ageRange.name} />
          </div>;
        })}
      </Card.Body>
    </Card>
  </>;
};

const DashboardPage = () => {
  return (
    <React.Fragment>
      <Helmet title="Visão geral" />
      <h1 className="h2 mb-4">Visão geral de clientes</h1>
      <Suspense fallback={<Loader />}>
        <DashboardCards />
      </Suspense>
    </React.Fragment>
  );
};

export default DashboardPage;
