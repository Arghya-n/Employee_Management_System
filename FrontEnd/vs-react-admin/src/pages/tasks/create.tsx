import TaskForm from '@/features/tasks/task-form';
import { TasksPartial } from '@/models/tasks-model';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';

const TaskCreate = () => {
  const initialValues: TasksPartial = {};
  
  return (
    <>
      <PageHeader
        title={'Projects'}
      />
      <PageContent>
        <TaskForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default TaskCreate;
