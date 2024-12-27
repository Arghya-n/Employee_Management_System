import { useEffect, useState } from 'react';
import { Button, Card, Col, DatePicker, Form, Input, Row, Select, Typography } from 'antd';
import TextArea from 'antd/es/input/TextArea';
import { SaveOutlined } from '@ant-design/icons';
import { validationMessage } from '@utils/helpers/message-helpers';
import { TasksPartial } from '@/models/tasks-model';

const { Title } = Typography;

interface TaskAssignmentFormProps {
  initialValues?: TasksPartial;
  isEditMode?: boolean;
}

const SAMPLE_PROJECT = [
  { label: 'Project 1', value: 'Project 1' },
  { label: 'Project 2', value: 'Project 2' },
  { label: 'Project 3', value: 'Project 3' },
];

const SAMPLE_USER = [
  { label: 'User 1', value: 'User 1' },
  { label: 'User 2', value: 'User 2' },
];

const TaskAssignmentForm = ({ initialValues, isEditMode = false }: TaskAssignmentFormProps) => {
  const [form] = Form.useForm();
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({ ...initialValues });
    }

    const handleResize = () => setWindowWidth(window.innerWidth);
    window.addEventListener('resize', handleResize);

    return () => window.removeEventListener('resize', handleResize);
  }, [initialValues, form, isEditMode]);

  // Adjust title font size based on window width
  const titleFontSize = windowWidth < 768 ? '1.2rem' : '1.5rem'; // Smaller title on mobile

  return (
    
      <Form
      form={form}
      layout="vertical"
      autoComplete={'off'}
      
      initialValues={initialValues}
    >
      <Card
        title={
          <div
            style={{
              fontSize: '1.2rem',
              whiteSpace: 'normal',
              wordWrap: 'break-word',
              textAlign: 'center',
            }}
          >
            Task Assignment Info
          </div>
        }
      >
        <Row gutter={24}>
          {/* Project */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item
              label="Project"
              name="project"
              rules={[{ required: !isEditMode, message: validationMessage('Project') }]}
            >
              <Select options={SAMPLE_PROJECT} placeholder="Select Project" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[16, 16]}>
          {/* User */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item
              label="User"
              name="user"
              rules={[{ required: !isEditMode, message: validationMessage('User') }]}
            >
              <Select options={SAMPLE_USER} placeholder="Select User" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[16, 16]}>
          {/* Task Title */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item
              label="Task Title"
              name="taskTitle"
              rules={[{ required: true, message: validationMessage('Task Title') }]}
            >
              <Input placeholder="Task Title" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[16, 16]}>
          {/* Assigned Date */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item
              label="Assigned Date"
              name="assignedDate"
              rules={[{ required: true, message: validationMessage('Assigned Date') }]}
            >
              <DatePicker placeholder="Assigned Date" style={{ width: '100%' }} />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[16, 16]}>
          {/* Description */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item label="Description" name="description">
              <TextArea rows={4} placeholder="Description" />
            </Form.Item>
          </Col>
        </Row>
      </Card>

      {/* Save Button */}
      <Row className="my-6">
        <Col span={24} className="text-right">
          <Button type="primary" htmlType="submit" icon={<SaveOutlined />}>
            Save changes
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default TaskAssignmentForm;
