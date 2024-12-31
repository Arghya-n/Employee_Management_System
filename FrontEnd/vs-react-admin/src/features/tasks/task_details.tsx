import TextArea from "antd/es/input/TextArea";
import { Button, Card, Col, Form, Input, Row } from "antd";
import { SaveOutlined } from "@ant-design/icons";

const TaskDetails = () => {
  //   const user = useAppSelector((state) => state.user); // Uncomment to access user data from the store

  return (
    <Card title="Task Information">
      <Form layout="vertical">
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item label="Task Title">
              <Input placeholder="Task Title" disabled />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="User Name">
              <Input placeholder="User Name" disabled />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Assigned Date">
              <Input placeholder="Assigned Date" disabled />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Description">
              <TextArea placeholder="Description" rows={4} disabled />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Percent Complete">
              <Input placeholder="Percent Complete" type="number" suffix="%" />
            </Form.Item>
          </Col>
        </Row>
        {/* Save Button */}
        <Row className="my-6">
          <Col span={24} className="text-right">
            <Button type="primary" htmlType="submit" icon={<SaveOutlined />}>
              Save changes
            </Button>
          </Col>
        </Row>
      </Form>
    </Card>
  );
};

export default TaskDetails;
