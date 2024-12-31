import { Space } from 'antd';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import TaskDetails from '@/features/tasks/task_details';

const AssignedTask = () => {
  return (
    <>
      <PageHeader
        title="Assigned Task"
        subTitle="Access and adjust your preferences conveniently on our this page"
      />
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: 'flex' }}>
          <TaskDetails />
        </Space>
      </PageContent>
    </>
  );
};

export default AssignedTask;
