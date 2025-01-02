import { useState } from "react";
import { Form, Input, Button } from "antd";
import { LockOutlined, UnlockOutlined, UserOutlined } from "@ant-design/icons";
import useAuth from "@hooks/use-auth";

interface LoginFormProps {
  email: string;
  password: string;
  role: string;
}

const LoginForm = () => {
  const [passwordVisible, setPasswordVisible] = useState(false);
  const { onLogin, isLoading } = useAuth();

  const onFinish = (values: LoginFormProps) => {
    onLogin({
      email: values.email,
      password: values.password,
      role: values.role,
    });
  };

  return (
    <Form
      layout="vertical"
      initialValues={{
        email: "s@gmail.com",
        password: "123456",
        role: "Admin",
        remember: true,
      }}
      requiredMark={false}
      onFinish={onFinish}
      style={{ width: "100%" }}
    >
      {/* Email Field */}
      <Form.Item
        name="email"
        label="Email"
        rules={[{ required: true, message: "Email is required" }]}
      >
        <Input prefix={<UserOutlined />} placeholder="Email" />
      </Form.Item>

      {/* Password Field */}
      <Form.Item
        name="password"
        label="Password"
        rules={[{ required: true, message: "Password is required" }]}
      >
        <Input.Password
          prefix={passwordVisible ? <UnlockOutlined /> : <LockOutlined />}
          placeholder="Password"
          iconRender={(visible) =>
            visible ? <UnlockOutlined /> : <LockOutlined />
          }
          visibilityToggle
          onClick={() => setPasswordVisible(!passwordVisible)}
        />
      </Form.Item>

      {/* Submit Button */}
      <Form.Item>
        <Button loading={isLoading} type="primary" htmlType="submit" block>
          Log in
        </Button>
      </Form.Item>
    </Form>
  );
};

export default LoginForm;
