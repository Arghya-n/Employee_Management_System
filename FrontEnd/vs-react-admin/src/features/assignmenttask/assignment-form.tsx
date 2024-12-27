import TextArea from 'antd/es/input/TextArea';
import { useEffect } from 'react';
import { Button, Card, Col, DatePicker, Form, Input, Row, Select } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
// import { useUserForm } from '@hooks/use-users';
import { TasksPartial } from '@/models/tasks-model';
import { validationMessage } from '@/utils/helpers/message-helpers';

interface TaskAssignmentFormProps {
  initialValues?: TasksPartial;
  isEditMode?: boolean;
}

const SAMPLE_PROJECT = [
  {
    label : 'Project 1',
    value : 'Project 1'
  },
  {
    label : 'Project 2',
    value : 'Project 2'
  },
  {
    label : 'Project 3',
    value : 'Project 3'
  }
];

const SAMPLE_USER = [
  {
    label : 'User 1',
    value : 'User 1'
  },
  {
    label : 'User 2',
    value : 'User 2'
  },
];

const TaskAssignmentForm = ({ initialValues, isEditMode = false }: TaskAssignmentFormProps) => {
  const [form] = Form.useForm();
  
  // const { onSaved, isLoading } = useUserForm();
  
  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues
      });
    }
  }, [initialValues, form, isEditMode]);
  
  // const onFinished = (values: User) => {
  //   values.id = isEditMode ? initialValues?.id ?? 0 : 0;
    
  //   const userData = _.omit(values, 'confirm_password');
  //   onSaved(userData);
  // };
  
  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete={'off'}
      initialValues={initialValues}
      >
      <Card title="Task Assignment Info">
        <Row gutter={24}>
          <Col span={24}>
            <Form.Item
              label="Project"
              name="project"
              rules={[{ required: !isEditMode, message: validationMessage('Project') }]}
            >
            <Select options={SAMPLE_PROJECT} placeholder="Select Project" />
            </Form.Item>
          </Col>

          <Col span={24}>
            <Form.Item
              label="User"
              name="user"
              rules={[{ required: !isEditMode, message: validationMessage('User') }]}
            >
            <Select options={SAMPLE_USER} placeholder="Select User" />
            </Form.Item>
          </Col>

          <Col span={24}>
            <Form.Item
              label="Task Title"
              name="taskTitle" 
              rules={[{ required: true, message: validationMessage('Task Title') }]}
            >
              <Input placeholder="Task Title" />
            </Form.Item>
          </Col>
          <Col span={24}>
            <Form.Item
              label="Assigned Date"
              name="assignedDate"
              rules={[{ required: true, message: validationMessage('Assigned Date') }]} 
            >
              <DatePicker placeholder="Assigned Date" />
            </Form.Item>
          </Col>
          <Col span={24}>
            <Form.Item
              label="Description"
              name="description" 
            >
              <TextArea rows={4} placeholder="Description" />
            </Form.Item>
          </Col>
        </Row>
      </Card>
      <Row className="my-6">
        <Col span={24} className="text-right">
          <Button
            type="primary"
            htmlType="submit"
            icon={<SaveOutlined />}
            >
            Save changes
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default TaskAssignmentForm;
