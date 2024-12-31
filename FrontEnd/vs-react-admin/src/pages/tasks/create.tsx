import TaskForm from '@/features/tasks/task-form';
import { TasksPartial } from '@/models/tasks-model';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import { Row, Col } from 'antd';

const TaskCreate = () => {
  const initialValues: TasksPartial = {};
  
  return (
    <>
      <PageHeader title={'Projects'} />
      <PageContent>
        <Row gutter={[16, 16]} justify="center">
          <Col xs={24} sm={22} md={18} lg={12} xl={10}>
            <TaskForm initialValues={initialValues} />
          </Col>
        </Row>
      </PageContent>
    </>
  );
};

export default TaskCreate;
