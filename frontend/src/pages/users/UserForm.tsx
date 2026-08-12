import { Suspense } from "react";
import { Helmet } from "react-helmet-async";
import { useNavigate, useParams } from "react-router-dom";
import { useSuspenseQuery } from "@tanstack/react-query";
import { Formik } from "formik";
import { Button, Card, Col, Form, Row } from "react-bootstrap";
import { NAVIGATION_PATH } from "@/constants";
import { ReactQueryKeys } from "@/constants/ReactQueryKeys";
import Loader from "@/components/Loader";
import { TextFormField } from "@/components/form/TextFormField/TextFormField";
import { TextFormFieldType } from "@/components/form/TextFormField/TextFormFieldType";
import UserService from "@/services/UserService";
import { User } from "@/types/api/User";
import { UserProfile, userProfileOptions } from "@/types/api/enums/UserProfile";
import { toastr } from "@/utils/toastr";
import yup from "@/utils/yup";

const INITIAL_VALUES: User = {
    username: "",
    password: "",
    profile: UserProfile.Operator,
};

const createSchemaValidation = yup.object().shape({
    username: yup.string().required("Usuário é obrigatório").max(50, "Usuário deve ter no máximo 50 caracteres"),
    password: yup.string().required("Senha é obrigatória").min(6, "Senha deve ter pelo menos 6 caracteres"),
    profile: yup.mixed<UserProfile>().oneOf(Object.values(UserProfile).filter(value => typeof value === "number") as UserProfile[], "Perfil é obrigatório").required("Perfil é obrigatório"),
});

const updateSchemaValidation = yup.object().shape({
    username: yup.string().required("Usuário é obrigatório").max(50, "Usuário deve ter no máximo 50 caracteres"),
    profile: yup.mixed<UserProfile>().oneOf(Object.values(UserProfile).filter(value => typeof value === "number") as UserProfile[], "Perfil é obrigatório").required("Perfil é obrigatório"),
});

const UserForm = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const isEditing = Boolean(id);

    const { data } = useSuspenseQuery<User>({
        queryKey: [ReactQueryKeys.USER, id ?? "new"],
        meta: {
            fetchFn: async () => isEditing ? await UserService.getById(id!) : INITIAL_VALUES,
        },
    });

    async function onSubmit(values: User) {
        try {
            if (isEditing) {
                await UserService.update(id!, { id, username: values.username, profile: values.profile });
                toastr({ title: "Usuário atualizado com sucesso", icon: "success" });
            } else {
                await UserService.create(values);
                toastr({ title: "Usuário criado com sucesso", icon: "success" });
            }

            navigate(NAVIGATION_PATH.USERS.LISTING.ABSOLUTE);
        } catch (err: any) {
            toastr({ title: "Erro", text: err.message, icon: "error" });
        }
    }

    const title = isEditing ? "Editar Usuário" : "Novo Usuário";

    return <>
        <Helmet title={title} />
        <Suspense fallback={<><Loader /><br /><br /></>}>
            <Card>
                <Card.Header>
                    <Card.Title>{title}</Card.Title>
                </Card.Header>
                <Card.Body>
                    <Formik
                        initialValues={data}
                        validationSchema={isEditing ? updateSchemaValidation : createSchemaValidation}
                        onSubmit={onSubmit}
                        enableReinitialize
                    >
                        {({ handleSubmit, handleChange, handleBlur, errors, values, isSubmitting, isValid }) => (
                            <Form noValidate onSubmit={handleSubmit}>
                                <Row>
                                    <Col md={4}>
                                        <TextFormField
                                            componentType={TextFormFieldType.INPUT}
                                            name="username"
                                            label="Usuário"
                                            required
                                            placeholder="Usuário"
                                            handleBlur={handleBlur}
                                            handleChange={handleChange}
                                            value={values.username}
                                            formikError={errors.username}
                                        />
                                    </Col>
                                    {!isEditing && <Col md={4}>
                                        <TextFormField
                                            componentType={TextFormFieldType.INPUT}
                                            name="password"
                                            label="Senha"
                                            required
                                            password
                                            placeholder="Senha"
                                            handleBlur={handleBlur}
                                            handleChange={handleChange}
                                            value={values.password}
                                            formikError={errors.password}
                                        />
                                    </Col>}
                                    <Col md={4}>
                                        <TextFormField
                                            componentType={TextFormFieldType.SELECT}
                                            name="profile"
                                            label="Perfil"
                                            required
                                            options={userProfileOptions()}
                                            handleBlur={handleBlur}
                                            handleChange={handleChange}
                                            value={values.profile}
                                            formikError={errors.profile as string}
                                        />
                                    </Col>
                                </Row>
                                <br />
                                <Button type="submit" variant="primary" disabled={!isValid || isSubmitting}>
                                    {isSubmitting ? "Salvando..." : "Salvar"}
                                </Button>
                                <Button variant="secondary" style={{ marginLeft: 5 }} onClick={() => navigate(NAVIGATION_PATH.USERS.LISTING.ABSOLUTE)}>
                                    Voltar
                                </Button>
                            </Form>
                        )}
                    </Formik>
                </Card.Body>
            </Card>
        </Suspense>
    </>;
};

export default UserForm;
