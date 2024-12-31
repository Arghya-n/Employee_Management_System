import TaskAssignmentForm from '@/features/assignmenttask/assignment-form';
import { TaskAssignmentPartial } from '@/models/task-assignment';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import { Row, Col } from 'antd';

const TaskAssignmentCreate = () => {
  const initialValues: TaskAssignmentPartial = {};

  return (
    <>
      <PageHeader title={'Create Task Assignment'} />
      <PageContent>
        <Row gutter={[16, 16]} justify="center">
          <Col xs={24} sm={22} md={18} lg={12} xl={10}>
            <TaskAssignmentForm initialValues={initialValues} />
          </Col>
        </Row>
      </PageContent>
    </>
  );
};

export default TaskAssignmentCreate;
