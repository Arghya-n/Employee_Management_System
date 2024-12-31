import TextArea from 'antd/es/input/TextArea';
import { Card, Col, Form, Input, Row } from 'antd';

const TaskDetails = () => {
//   const user = useAppSelector((state) => state.user); // Uncomment to access user data from the store

  return (
    <Card title="Task Information">
      <Form layout="vertical">
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item label="Task Title">
              <Input placeholder="Task Title" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="User Name">
              <Input placeholder="User Name" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Assigned Date">
              <Input placeholder="Assigned Date" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Description">
              <TextArea placeholder="Description" rows={4} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Percent Complete">
              <Input placeholder="Percent Complete" type="number" suffix="%" />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Card>
  );
};

export default TaskDetails;
