import { Link } from "react-router-dom";
import {
  MenuProps,
  TableProps,
  Dropdown,
  Button,
  Typography,
  Table,
} from "antd";
import { EditOutlined, MoreOutlined } from "@ant-design/icons";
import { Tasks } from "@models/tasks-model";

const { Text } = Typography;

const getActions = (taskId: number): MenuProps["items"] => {
  return [
    {
      key: `edit-${taskId}`,
      label: (
        <Link to={`/tasks/${taskId}`}>
          <EditOutlined /> Edit
        </Link>
      ),
    },
  ];
};

const columns: TableProps<Tasks>["columns"] = [
  {
    title: "Title",
    dataIndex: "title",
    sorter: true,
    key: "title",
    render: (_, record) => (
      <Link to={`/users/${record.projectId}`}>{record.title}</Link>
    ),
    width: 200, // Fixed width to prevent wrapping
  },
  {
    title: "Start Date",
    key: "start_date",
    render: (_, record) => <Text>{record.startDate}</Text>,
    width: 150, // Fixed width to prevent wrapping
  },
  {
    title: "End Date",
    key: "end_date",
    render: (_, record) => <Text>{record.endDate}</Text>,
    width: 150, // Fixed width to prevent wrapping
  },
  {
    title: "Action",
    key: "action",
    align: "center",
    render: (_, record) => (
      <Dropdown
        menu={{ items: getActions(record.projectId!) }}
        overlayClassName="grid-action"
      >
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    ),
    width: 100, // Fixed width for the action column
  },
];

const TableComponent = ({ data }: { data: Tasks[] }) => (
  <div style={{ overflowX: "auto", width: "100%" }}>
    <Table
      columns={columns}
      dataSource={data}
      scroll={{ x: "max-content" }} // Enables horizontal scrolling if necessary
      //pagination={false} // Optional: turn off pagination if you want to show all data at once
      size="middle" // Optional: adjust table size for better readability
    />
  </div>
);

export { columns, TableComponent };
