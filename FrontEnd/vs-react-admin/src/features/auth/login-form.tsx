import { useState } from "react";
import { Form, Input, Button, Checkbox, Typography, Select } from "antd";
import { LockOutlined, UnlockOutlined, UserOutlined } from "@ant-design/icons";
import useAuth from "@hooks/use-auth";

const { Link } = Typography;
const { Option } = Select;

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
        email: "john@mail.com",
        password: "changeme",
        remember: true,
      }}
      requiredMark={false}
      onFinish={onFinish}
      style={{ width: "100%" }}
    >
      {/* Role Selection */}
      <Form.Item
        name="role"
        label="Log in as"
        rules={[{ required: true, message: "Selection is required" }]}
      >
        <Select placeholder="Select role">
          <Option value="admin">Admin</Option>
          <Option value="user">User</Option>
        </Select>
      </Form.Item>

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

      {/* Remember Me and Forgot Password */}
      <Form.Item>
        <div className="flex items-center justify-between">
          <Form.Item name="remember" valuePropName="checked" noStyle>
            <Checkbox>Remember me</Checkbox>
          </Form.Item>
          <Link href="/forget-password">Forgot password</Link>
        </div>
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
