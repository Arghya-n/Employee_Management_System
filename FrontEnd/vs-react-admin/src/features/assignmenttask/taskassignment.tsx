import { Link } from 'react-router-dom';
import { MenuProps, TableProps, Dropdown, Button, Typography, Tag } from 'antd';
import { EditOutlined, MoreOutlined } from '@ant-design/icons';
import { TaskAssignment } from '@/models/task-assignment';

const { Text } = Typography;

const getActions = (taskId: number): MenuProps['items'] => {
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

const columns: TableProps<TaskAssignment>['columns'] = [
  {
    title: 'Task Title',
    dataIndex: 'taskTitle',
    sorter: true,
    key: 'taskTitle',
    render: (_, record) => <Text>{record.taskTitle}</Text>,
    width: 200, // Fixed width to prevent wrapping
  },
  {
    title: 'User ID',
    key: 'userId',
    render: (_, record) => <Text>{record.userId}</Text>,
    width: 150, // Fixed width to prevent wrapping
  },
  {
    title: 'Assigned Date',
    key: 'assignedDate',
    render: (_, record) => <Text>{record.assignedDate}</Text>,
    width: 150, // Fixed width to prevent wrapping
  },
  {
    title: 'Description',
    key: 'description',
    render: (_, record) => <Text>{record.description}</Text>,
    width: 250, // Fixed width to prevent wrapping
  },
  {
    title: 'Percent Complete',
    key: 'percentComplete',
    render: (_, record) => <Text>{record.percentComplete}</Text>,
    width: 150, // Fixed width to prevent wrapping
  },
  {
    title: 'Action',
    key: 'action',
    align: 'center',
    render: (_, record) => (
      <Dropdown menu={{ items: getActions(record.assignementid!) }} overlayClassName="grid-action">
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    ),
    width: 100, // Fixed width for the action column
  },
];

export { columns };
