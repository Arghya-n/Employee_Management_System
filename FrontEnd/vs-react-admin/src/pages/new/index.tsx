import React from 'react';
import { Link } from 'react-router-dom';
import { Button, Row, Col } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import TaskAssignmentTable from '@/features/tasks/table';

const TaskAssignment = () => {
  return (
    <>
      {/* Page Header Section */}
      <PageHeader
        title="Task Assignment"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        {/* Button Section */}
        <Row justify="end" gutter={[16, 16]}>
  <Col xs={24} sm={12} md={8} lg={6}>
    <Link to="/taskAssignment/create">
      <Button
        type="primary"
        icon={<PlusCircleOutlined />}
        block
        style={{
          width: '110%', 
          display: 'flex', 
          justifyContent: 'center', 
          alignItems: 'center', 
        }}
      >
        Create Assignment
      </Button>
    </Link>
  </Col>
</Row>


      </PageHeader>

      {/* Main Content Section */}
      <PageContent>
        <TaskAssignmentTable />
      </PageContent>
    </>
  );
};

export default TaskAssignment;
