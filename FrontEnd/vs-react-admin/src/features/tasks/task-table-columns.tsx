import { Link } from 'react-router-dom';
import { MenuProps, TableProps, Tag, Dropdown, Button, Typography } from 'antd';
import { EditOutlined, MoreOutlined } from '@ant-design/icons';
import { Tasks } from '@models/tasks-model';

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

const columns : TableProps<Tasks>['columns'] = [
  {
    title: 'Title',
    dataIndex: 'title',
    sorter: true,
    key: 'title',
    render: (_, record) => (
        <Text>{record.title}</Text>
    )
  },
  {
    title: 'Start Date',
    key: 'start_date',
    render: (_, record) => (
      <Text>{record.start_date}</Text>
    )
  },
  {
    title: 'End Date',
    key: 'end_date',
    render: (_, record) => (
      <Text>{record.end_date}</Text>
    )
  },
  {
    title: 'Status',
    key: 'role',
    render: (_, record) => (
      <Tag color="geekblue" className="uppercase">{record.status}</Tag>
    )
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    align: 'center',
    width: 100,
    render: (_, record) => (
      <Dropdown menu={{items: getActions(record.id!)}} overlayClassName="grid-action">
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    )
  }
];

export { columns };