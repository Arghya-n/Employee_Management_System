import { useState } from 'react';
import { Table, Input, Card } from 'antd';
import useFilter from '@hooks/utility-hooks/use-filter';
import { columns } from '../assignmenttask/taskassignment';

const TaskAssignmentTable = () => {
//   const {
//     isLoading,
//     data
//   } = useUsers();
  
  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);

  const onSearchHandle = (value: string) => {
    setQueryParams({
      ...getQueryParams(),
      search: value
    });
  };
  
  return (
    <Card
      title={'Task Assignmemnt'}
      extra={(
        <div className="my-6">
          <Input.Search
            placeholder={'Search'}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onSearch={onSearchHandle}
            allowClear />
        </div>
      )}
    >
      <Table
        columns={columns}
        pagination={false}
        onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="id"
        bordered
      />
      {/*<div className={'flex justify-end mt-4'}>*/}
      {/*  <PaginationWrapper totalItems={data?.totalNumberOfElemements || 0} />*/}
      {/*</div>*/}
    </Card>
  );
};

export default TaskAssignmentTable;
