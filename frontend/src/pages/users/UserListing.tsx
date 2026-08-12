import { Suspense } from "react";
import { Button, Card } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { NAVIGATION_PATH } from "@/constants";
import DataTable from "@/components/DataTable";
import { ActionItemType, CrudActions } from "@/components/CrudActions";
import Loader from "@/components/Loader";
import UserService from "@/services/UserService";
import { User } from "@/types/api/User";
import { UserFilter } from "@/types/api/filters/UserFilter";
import { UserProfile } from "@/types/api/enums/UserProfile";
import { mountRoute } from "@/utils/mountRoute";

const UserListing = () => {
    const navigate = useNavigate();
    return <>
        <div style={{ display: "flex", justifyContent: "flex-end", alignItems: "center", gap: "8px", margin: "10px 0" }}>
            <Link to={NAVIGATION_PATH.USERS.CREATE.ABSOLUTE}>
                <Button style={{ maxWidth: "fit-content", float: "right" }}>Adicionar</Button>
            </Link>
        </div>
        <Card>
            <Card.Header>
                <Card.Title>Usuários</Card.Title>
            </Card.Header>
            <Suspense fallback={<><Loader /><br /><br /></>}>
                <DataTable<User, UserFilter>
                    thin
                    columns={[
                        { Header: "Usuário", accessor: "username" },
                        {
                            Header: "Perfil",
                            accessor: "profile",
                            Cell: ({ value }) => UserProfile[value as UserProfile],
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
                                            handler: (user) => {
                                                if (user.id) {
                                                    navigate(mountRoute(NAVIGATION_PATH.USERS.EDIT.ABSOLUTE, { id: user.id }));
                                                }
                                            },
                                        },
                                    ]}
                                />
                            ),
                        },
                    ]}
                    query={async () => await UserService.getAll()}
                    queryName={["user", "listing"]}
                />
            </Suspense>
        </Card>
    </>;
};

export default UserListing;
