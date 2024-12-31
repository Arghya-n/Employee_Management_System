import { useEffect, useState } from "react";
import { Table, Input, Card, Row, Col, Alert } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./task-table-columns";

const TaskTable = () => {
  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);

  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTasks = async () => {
      try {
        const response = await fetch("https://192.168.10.175:7033/api/Project");
        const data = await response.json();
        setTasks(data);
      } catch (error) {
        setError("Error fetching data");
      } finally {
        setLoading(false);
      }
    };

    fetchTasks();
  }, []);

  const onSearchHandle = (value: string) => {
    setQueryParams({
      ...getQueryParams(),
      search: value,
    });
  };

  if (error) {
    return <Alert message="Error" description={error} type="error" showIcon />;
  }

  return (
    <Card
      title={"Projects"}
      extra={
        <Row gutter={[16, 16]} justify="end">
          <Col xs={24} sm={12} md={8}>
            <Input.Search
              placeholder={"Search"}
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              onSearch={onSearchHandle}
              allowClear
              style={{ width: "100%" }}
            />
          </Col>
        </Row>
      }
    >
      <Table
        columns={columns}
        pagination={false}
        loading={loading}
        onChange={sortTableColumn}
        dataSource={tasks}
        scroll={{ x: "max-content", y: 350 }} // x allows horizontal scroll for responsive table
        rowKey="id"
        bordered
        //responsive
      />
    </Card>
  );
};

export default TaskTable;
