import { Card, Col, Form, Input, Row, Tag } from "antd";
import { useAppSelector } from "@/store";
import { useState } from "react";
import { LockOutlined, UnlockOutlined } from "@ant-design/icons";

const UserDetails = () => {
  const user = useAppSelector((state) => state.user);

  const [passwordVisible, setPasswordVisible] = useState(false);

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
                visibilityToggle={{
                  visible: passwordVisible,
                  onVisibleChange: setPasswordVisible,
                }}
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
        </Row>
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item label="Status">
              <Tag key="status" color="geekblue">
                {user.role}
              </Tag>
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Card>
  );
};

export default UserDetails;
