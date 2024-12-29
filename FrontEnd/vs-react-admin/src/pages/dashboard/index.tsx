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
<<<<<<< HEAD
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
=======
        <Row gutter={[16, 16]} className='mb-4'>
          <Col md={6} sm={12} xs={24}>
            <Card title={
              <Space>
                <UserOutlined style={{color: 'red'}} />
                Total Employee
              </Space>
              } bordered={false}>
              300
            </Card>
          </Col>
          <Col md={6} sm={12} xs={24}>
            <Card title={
              <Space>
                <ProjectOutlined style={{color: 'green'}} />
                Total Projects
              </Space>
            } bordered={false}>
              20
            </Card>
          </Col>
          <Col md={6} sm={12} xs={24}>
            <Card title={
              <Space>
                <UserAddOutlined style={{color: 'yellow'}} />
                Free Employee
              </Space>
            } bordered={false}>
              100
            </Card>
          </Col>
          <Col md={6} sm={12} xs={24}>
            <Card title={
              <Space>
                <UserSwitchOutlined style={{color: 'blue'}} />
                Assigned Employee
              </Space>
            } bordered={false}>
              200
>>>>>>> f52a0c520f14ee0ffb2e1ee1f63c99acea2e8914
            </Card>
          </Col>
        </Row>
        <Row gutter={[16, 16]} style={{ minHeight: '400px' }}>
<<<<<<< HEAD
          <Col xs={24} lg={12}>
            <Card title="Working Stack" bordered={false} style={{ height: '100%' }}>
              <BarChart data={chartData} title="" />
            </Card>
          </Col>
          <Col xs={24} lg={12}>
            <Card title="Employee Status" bordered={false} style={{ height: '100%' }}>
              <PieChartComponent />
=======
          <Col md={12} sm={24} xs={24}>
            <Card title="Working Stack" bordered={false} style={{height: '100%'}}>
              <BarChart data={chartData} title="" />
            </Card>
          </Col>
          <Col md={12} sm={24} xs={24}>
            <Card title="Employee Status" bordered={false} style={{height: '100%'}}>
              <PieChartComponent/>
>>>>>>> f52a0c520f14ee0ffb2e1ee1f63c99acea2e8914
            </Card>
          </Col>
        </Row>
      </PageContent>
    </>
  );
};

export default Dashboard;
