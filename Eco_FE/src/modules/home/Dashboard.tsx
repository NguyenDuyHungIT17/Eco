import { Card, Descriptions, Empty, Space, Tag, Typography } from 'antd';
import { useAppSelector } from 'store/hooks';

export function Component() {
  const { profile, roles, permissions } = useAppSelector((s) => s.oauth);

  const tags = (values: string[]) =>
    values.length > 0 ? (
      <Space wrap>
        {values.map((v) => (
          <Tag key={v}>{v}</Tag>
        ))}
      </Space>
    ) : (
      <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="Chưa có" />
    );

  return (
    <Space direction="vertical" size="large" className="page">
      <Typography.Title level={3}>Xin chào, {profile?.username}</Typography.Title>
      <Card title="Thông tin tài khoản">
        <Descriptions column={1}>
          <Descriptions.Item label="Tên đăng nhập">{profile?.username}</Descriptions.Item>
          <Descriptions.Item label="Email">{profile?.email}</Descriptions.Item>
          <Descriptions.Item label="Vai trò">{tags(roles)}</Descriptions.Item>
          <Descriptions.Item label="Quyền">{tags(permissions)}</Descriptions.Item>
        </Descriptions>
      </Card>
    </Space>
  );
}
