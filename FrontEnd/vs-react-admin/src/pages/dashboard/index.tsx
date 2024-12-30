import { Card, Row, Col, Space } from 'antd';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import { ProjectOutlined, UserAddOutlined, UserOutlined, UserSwitchOutlined } from '@ant-design/icons';
import BarChart from '@/components/shared/ant-design-chart';
import PieChartComponent from '@/components/shared/pie-chart';

const Dashboard = () => {
  const chartData = [
    { name: 'React', value: 50 },
    { name: 'DotNet', value: 30 },
    { name: 'PHP', value: 20 },
    { name: 'Python', value: 40 },
    { name: 'Java', value: 60 },
  ];

  return (
    <>
      <PageHeader title="Dashboard" subTitle="Quick access to analytical insights" />
      <PageContent>
        <Row gutter={[16, 16]} className="mb-4">
          <Col xs={24} sm={12} md={6}>
            <Card
              title={
                <Space>
                  <UserOutlined style={{ color: 'red' }} />
                  Total Employee
                </Space>
              }
              bordered={false}
            >
              <div>300</div>
            </Card>
          </Col>
          <Col xs={24} sm={12} md={6}>
            <Card
              title={
                <Space>
                  <ProjectOutlined style={{ color: 'green' }} />
                  Total Projects
                </Space>
              }
              bordered={false}
            >
              <div>20</div>
            </Card>
          </Col>
          <Col xs={24} sm={12} md={6}>
            <Card
              title={
                <Space>
                  <UserAddOutlined style={{ color: 'yellow' }} />
                  Free Employee
                </Space>
              }
              bordered={false}
            >
              <div>100</div>
            </Card>
          </Col>
          <Col xs={24} sm={12} md={8} lg={6} >
            <Card
              title={
                <Space>
                  <UserSwitchOutlined style={{ color: 'blue' }} />
                  Assigned Employee
                </Space>
              }
              bordered={false}
            >
              <div>200</div>
            </Card>
          </Col>
        </Row>
        <Row gutter={[16, 16]} style={{ minHeight: '400px' }}>
          <Col xs={24} lg={12}>
            <Card title="Working Stack" bordered={false} style={{ height: '100%' }}>
              <BarChart data={chartData} title="" />
            </Card>
          </Col>
          <Col xs={24} lg={12}>
            <Card title="Employee Status" bordered={false} style={{ height: '100%' }}>
              <PieChartComponent />
            </Card>
          </Col>
        </Row>
      </PageContent>
    </>
  );
};

export default Dashboard;
