import { useState } from "react";
import { Table, Input, Card, Row, Col } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./task-table-columns";
import { useProjects } from "@/hooks/use-projects";

const TaskTable = () => {
  const { data, isLoading } = useProjects();
  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);

  const onSearchHandle = (value: string) => {
    setQueryParams({
      ...getQueryParams(),
      search: value,
    });
  };

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
        loading={isLoading}
        onChange={sortTableColumn}
        dataSource={data || []}
        scroll={{ x: "max-content", y: 350 }} // x allows horizontal scroll for responsive table
        rowKey="id"
        bordered
        //responsive
      />
    </Card>
  );
};

export default TaskTable;
