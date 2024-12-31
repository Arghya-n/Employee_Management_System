import TextArea from 'antd/es/input/TextArea';
import { useEffect } from 'react';
import { Button, Card, Col, DatePicker, Form, Input, Row } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { validationMessage } from '@utils/helpers/message-helpers';
import { TasksPartial } from '@/models/tasks-model';

interface TaskFormProps {
  initialValues?: TasksPartial;
  isEditMode?: boolean;
}

const TaskForm = ({ initialValues, isEditMode = false }: TaskFormProps) => {
  const [form] = Form.useForm();

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues,
      });
    }
  }, [initialValues, form, isEditMode]);

  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete="off"
      initialValues={initialValues}
      //style={{ maxWidth: '600px', margin: '0 auto' }}
    >
      <Card title="Project Info">
        <Row gutter={[16, 16]}>
          {/* Project Title */}
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item
              label="Project Title"
              name="title"
              rules={[{ required: true, message: validationMessage('Project Title') }]}
            >
              <Input placeholder="Project Title"
              
               />
            </Form.Item>
          </Col>
        </Row>

          {/* Description */}
          <Row gutter={[16, 16]}>
          <Col xs={24} sm={24} md={24} lg={24} xl={24}>
            <Form.Item label="Description" name="description">
              <TextArea rows={4} placeholder="Description"
              //style={{ width:'800px' }} 
              />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={[16, 16]}>
          {/* Start Date */}
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="Start Date"
              name="startDate"
              rules={[{ required: true, message: validationMessage('Start Date') }]}
            >
              <DatePicker placeholder="Start Date" style={{ width: '100%' }} />
            </Form.Item>
          </Col>

          {/* End Date */}
          <Col xs={24} sm={12} md={12} lg={12} xl={12}>
            <Form.Item
              label="End Date"
              name="endDate"
              rules={[{ required: true, message: validationMessage('End Date') }]}
            >
              <DatePicker placeholder="End Date" style={{ width: '100%' }} />
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

export default TaskForm;
