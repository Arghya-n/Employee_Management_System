import { Link } from 'react-router-dom';
import { Button } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import TaskAssignmentTable from '@/features/tasks/table';

const Tasks = () => {
  return (
    <>
      <PageHeader
        title="Task Assignment"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        <Link to={'/tasks/create'}>
          <Button type={'primary'} icon={<PlusCircleOutlined />}>
            Create Assignment
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <TaskAssignmentTable />
      </PageContent>
    </>
  );
};

export default Tasks;
