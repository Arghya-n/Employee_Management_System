import React, { useEffect } from 'react';
import { Button, Card, Col, DatePicker, Form, Input, Row, Select } from 'antd';
import TextArea from 'antd/es/input/TextArea';
import { SaveOutlined } from '@ant-design/icons';
import { TasksPartial } from '@/models/tasks-model';
import { validationMessage } from '@/utils/helpers/message-helpers';

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

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({ ...initialValues });
    }
  }, [initialValues, form]);

  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete="off"
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
        <Row gutter={[24, 24]} justify="center">
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="Project"
              name="project"
              rules={[{ required: !isEditMode, message: validationMessage('Project') }]}
            >
              <Select options={SAMPLE_PROJECT} placeholder="Select Project" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[24, 24]} justify="center">
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="User"
              name="user"
              rules={[{ required: !isEditMode, message: validationMessage('User') }]}
            >
              <Select options={SAMPLE_USER} placeholder="Select User" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[24, 24]} justify="center">
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="Task Title"
              name="taskTitle"
              rules={[{ required: true, message: validationMessage('Task Title') }]}
            >
              <Input placeholder="Task Title" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[24, 24]} justify="center">
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="Assigned Date"
              name="assignedDate"
              rules={[{ required: true, message: validationMessage('Assigned Date') }]}
            >
              <DatePicker placeholder="Assigned Date" style={{ width: '100%' }} />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[24, 24]} justify="center">
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item label="Description" name="description">
              <TextArea rows={4} placeholder="Description" />
            </Form.Item>
          </Col>
        </Row>
      </Card>

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
