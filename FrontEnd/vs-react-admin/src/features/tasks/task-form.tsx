import _ from 'lodash';
import { useEffect } from 'react';
import { Button, Card, Col, DatePicker, Form, Input, Row } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
// import { useUserForm } from '@hooks/use-users';
import { validationMessage } from '@utils/helpers/message-helpers';
import { TasksPartial } from '@/models/tasks-model';
import TextArea from 'antd/es/input/TextArea';

interface TaskFormProps {
  initialValues?: TasksPartial;
  isEditMode?: boolean;
}

const TaskForm = ({ initialValues, isEditMode = false }: TaskFormProps) => {
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
      <Card title="Project Info">
        <Row gutter={24}>
          <Col span={24}>
            <Form.Item
              label="Project Title"
              name="title"
              rules={[{ required: true, message: validationMessage('Project Title') }]}
            >
              <Input placeholder="Project Title" />
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
          <Col span={12}>
            <Form.Item
              label="Start Date"
              name="startDate"
              rules={[
                { required: true, message: validationMessage('Start Date') }
              ]}
            >
              <DatePicker placeholder="Start Date" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="End Date"
              name="endDate"
              rules={[
                { required: true, message: validationMessage('End Date') }
              ]}
            >
              <DatePicker placeholder="End Date" />
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

export default TaskForm;
