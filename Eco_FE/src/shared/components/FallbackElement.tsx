import { Spin } from 'antd';

// Hiển thị trong lúc router đang tải module (lazy)
export function FallbackElement() {
  return (
    <div className="fallback">
      <Spin size="large" />
    </div>
  );
}
