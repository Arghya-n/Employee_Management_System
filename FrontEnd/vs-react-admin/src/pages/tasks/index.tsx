import { Link } from 'react-router-dom';
import { Button } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import TaskTable from '@/features/tasks/task-table';

const Tasks = () => {
  return (
    <>
      <PageHeader
        title="Tasks"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        <Link to={'/tasks/create'}>
          <Button type={'primary'} icon={<PlusCircleOutlined />}>
            Create Task
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <TaskTable />
      </PageContent>
    </>
  );
};

export default Tasks;
