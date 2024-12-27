import { useParams } from 'react-router-dom';
import { Spin } from 'antd';
import UserForm from '@features/users/user-form';
import { useUser } from '@hooks/use-users';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import { Row, Col } from 'antd';

const UserEdit = () => {
  const params = useParams();
  const userId = Number(params.id);
  const { isLoading, user } = useUser(userId);
  
  return (
    <>
      <PageHeader
        title={'Edit User'}
      />
      <PageContent>
        <Spin spinning={isLoading}>
          <Row justify="center" gutter={[16, 16]}>
            <Col xs={24} sm={20} md={18} lg={14} xl={12}>
              <UserForm initialValues={user} isEditMode />
            </Col>
          </Row>
        </Spin>
      </PageContent>
    </>
  );
};

export default UserEdit;
