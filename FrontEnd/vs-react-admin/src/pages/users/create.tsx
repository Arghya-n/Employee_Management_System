import UserForm from "@features/users/user-form";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { UserPartial } from "@models/user-model";
import { Row, Col } from "antd";

const UserCreate = () => {
  const initialValues: UserPartial = {};

  return (
    <>
      <PageHeader title="Create User" />
      <PageContent>
        <Row justify="center" gutter={[16, 16]}>
          <Col xs={24} sm={20} md={16} lg={12} xl={10}>
            <UserForm initialValues={initialValues} />
          </Col>
        </Row>
      </PageContent>
    </>
  );
};

export default UserCreate;
