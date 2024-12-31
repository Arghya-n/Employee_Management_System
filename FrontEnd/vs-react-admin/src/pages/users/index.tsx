import { Link } from "react-router-dom";
import { Button, Row, Col } from "antd";
import { PlusCircleOutlined } from "@ant-design/icons";
import UserTable from "@features/users/user-table";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";

const Users = () => {
  return (
    <>
      <PageHeader
        title="Employees"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        <Row justify="end" gutter={[16, 16]}>
          <Col>
            <Link to={"/users/create"}>
              <Button type={"primary"} icon={<PlusCircleOutlined />} block>
                Create User
              </Button>
            </Link>
          </Col>
        </Row>
      </PageHeader>
      <PageContent>
        <UserTable />
      </PageContent>
    </>
  );
};

export default Users;
