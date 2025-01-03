import { Link } from "react-router-dom";
import {
  MenuProps,
  TableProps,
  Tag,
  Dropdown,
  Button,
  Typography,
  Table,
} from "antd";
import { EditOutlined, MoreOutlined } from "@ant-design/icons";
import { User } from "@models/user-model";

const { Text } = Typography;

// Dummy action menu for editing
const getActions = (userId: number): MenuProps["items"] => {
  return [
    {
      key: `edit-${userId}`,
      label: (
        <Link to={`/users/${userId}`}>
          <EditOutlined /> Edit
        </Link>
      ),
    },
  ];
};

// Table columns with responsive settings
const columns: TableProps<User>["columns"] = [
  {
    title: "Name",
    dataIndex: "name",
    sorter: true,
    key: "name",
    render: (_, record) => (
      <Link to={`/users/${record.employeeId}`}>
        {record.name}
        <Text>{record.employeeId}</Text>
      </Link>
    ),
    responsive: ["xs", "sm", "md", "lg", "xl"],
  },
  {
    title: "Email",
    key: "email",
    render: (_, record) => <Text>{record.email}</Text>,
    responsive: ["xs", "sm", "md", "lg", "xl"],
  },
  {
    title: "Role",
    key: "role",
    render: (_, record) => (
      <Tag color="geekblue" className="uppercase">
        {record.role}
      </Tag>
    ),
    responsive: ["xs", "sm", "md", "lg", "xl"],
  },
  {
    title: "Working Stack",
    key: "workingStack",
    render: (_, record) => <Text>{record.stack}</Text>,
    responsive: ["xs", "sm", "md", "lg", "xl"],
  },
  {
    title: "Action",
    key: "action",
    align: "center",
    render: (_, record) => (
      <Dropdown
        menu={{ items: getActions(record.employeeId!) }}
        overlayClassName="grid-action"
      >
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    ),
    responsive: ["xs", "sm", "md", "lg", "xl"], // Visible on all screen sizes
  },
];

const TableComponent = ({ data }: { data: User[] }) => (
  <Table
    columns={columns}
    dataSource={data}
    scroll={{ x: "max-content" }} // Allows horizontal scrolling for larger tables
    //pagination={false} // Optional: turn off pagination if needed
    //size="middle" // Optional: adjust table size for readability
  />
);

export { columns, TableComponent };
