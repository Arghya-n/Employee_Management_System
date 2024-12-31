import { useEffect, useState } from "react";
import { Table, Input, Card, Alert } from "antd";
import { useUsers } from "@hooks/use-users";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./user-table-columns";

const UserTable = () => {
  // const { isLoading, data } = useUsers();

  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);

  const onSearchHandle = (value: string) => {
    setQueryParams({
      ...getQueryParams(),
      search: value,
    });
  };

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await fetch(
          "https://192.168.10.175:7033/api/Employee"
        );
        const data = await response.json();
        setUsers(data);
      } catch (error) {
        setError("Error fetching data");
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  if (error) {
    return <Alert message="Error" description={error} type="error" showIcon />;
  }

  return (
    <Card
      title={"Employee List"}
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
        dataSource={users}
        loading={loading}
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
