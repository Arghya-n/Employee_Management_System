import { Card, Col, Form, Input, Row, Tag } from 'antd';
import { useAppSelector } from '@/store';

const UserDetails = () => {
  const user = useAppSelector((state) => state.user);
  
  return (
    <Card title="Information">
      <Form layout="vertical">
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item label="Name">
              <Input value={user.name} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Email">
              <Input value={user.email} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Password">
              <Input.Password
                value={user.password}
                visibilityToggle
                iconRender={(visible) =>
                  visible ? (
                    <span role="img" aria-label="hide">
                      👁️
                    </span>
                  ) : (
                    <span role="img" aria-label="show">
                      👁️‍🗨️
                    </span>
                  )
                }
              />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Phone Number">
              <Input value={user.phoneNumber} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Working Stack">
              <Input value={user.workingStack} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="Role">
              <Input value={user.role} />
            </Form.Item>
          </Col>


          
        </Row>
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item label="Status">
              <Tag key='status' color="geekblue">{user.role}</Tag>
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Card>
  );
};

export default UserDetails;
