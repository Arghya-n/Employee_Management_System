import { Link } from "react-router-dom";
import { Button, Row, Col } from "antd";
import { PlusCircleOutlined } from "@ant-design/icons";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import TaskTable from "@/features/tasks/task-table";

const Tasks = () => {
  return (
    <>
      <PageHeader
        title="Projects"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        <Row justify="end" gutter={[16, 16]}>
          <Col xs={24} sm={12} md={8}>
            <Link to={"/tasks/create"}>
              <Button type="primary" icon={<PlusCircleOutlined />} block>
                Create Project
              </Button>
            </Link>
          </Col>
        </Row>
      </PageHeader>

      <PageContent>
        <TaskTable />
      </PageContent>
    </>
  );
};

export default Tasks;
