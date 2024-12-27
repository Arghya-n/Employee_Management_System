import { useState } from "react";
import { Table, Input, Card } from "antd";
import { useUsers } from "@hooks/use-users";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./user-table-columns";

const UserTable = () => {
  const { isLoading, data } = useUsers();

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
      title={"Users"}
      extra={
        <div className="my-6">
          <Input.Search
            placeholder={"Search"}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onSearch={onSearchHandle}
            allowClear
            style={{ maxWidth: 400, width: "100%" }} // Ensures the search input is responsive
          />
        </div>
      }
    >
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        onChange={sortTableColumn}
        scroll={{ x: 1200, y: 350 }} // Allow horizontal scrolling for large tables and vertical scrolling for rows
        rowKey="id"
        bordered
      />
    </Card>
  );
};

export default UserTable;
