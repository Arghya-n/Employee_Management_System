import TaskAssignmentForm from '@/features/assignmenttask/assignment-form';
import { TaskAssignmentPartial } from '@/models/task-assignment';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';

const TaskAssignmentCreate = () => {
  const initialValues: TaskAssignmentPartial = {};
  
  return (
    <>
      <PageHeader
        title={'Create Task Assignment'}
      />
      <PageContent>
        <TaskAssignmentForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default TaskAssignmentCreate;
