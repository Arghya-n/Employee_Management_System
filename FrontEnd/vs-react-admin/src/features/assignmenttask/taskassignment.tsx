import { Link } from 'react-router-dom';
import { MenuProps, TableProps, Dropdown, Button, Typography } from 'antd';
import { EditOutlined, MoreOutlined } from '@ant-design/icons';
import { TaskAssignment } from '@/models/task-assignment';

const { Text } = Typography;

const getActions = (taskId: number): MenuProps['items'] => {
  return [
    {
      key: `edit-${taskId}`,
      label: <Link to={`/tasks/${taskId}`}>
        <EditOutlined /> Edit
      </Link>,
    }
  ];
};

const columns : TableProps<TaskAssignment>['columns'] = [
  {
    title: 'Task ID',
    dataIndex: 'taskId',
    sorter: true,
    key: 'taskId',
    render: (_, record) => (
        <Text>{record.taskId}</Text>
    )
  },
  {
    title: 'User ID',
    key: 'userId',
    render: (_, record) => (
      <Text>{record.userId}</Text>
    )
  },
  {
    title: 'Assigned Date',
    key: 'assignedDate',
    render: (_, record) => (
      <Text>{record.assignedDate}</Text>
    )
  },
  {
    title: 'Description',
    key: 'description',
    render: (_, record) => (
        <Text>{record.description}</Text>
    )
  },
  {
    title: 'Percent Complete',
    key: 'percentComplete',
    render: (_, record) => (
        <Text>{record.percentComplete}</Text>
    )
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    align: 'center',
    width: 100,
    render: (_, record) => (
      <Dropdown menu={{items: getActions(record.assignementid!)}} overlayClassName="grid-action">
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    )
  }
];

export { columns };