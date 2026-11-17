import { useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from 'shared/services';
import { FallbackElement } from 'shared/components';

export function Component() {
  const navigate = useNavigate();
  // StrictMode chạy effect 2 lần ở dev → chặn gọi revoke-token 2 lần
  const started = useRef(false);

  useEffect(() => {
    if (started.current) {
      return;
    }
    started.current = true;
    authService.logout().finally(() => navigate('/login', { replace: true }));
  }, [navigate]);

  return <FallbackElement />;
}
