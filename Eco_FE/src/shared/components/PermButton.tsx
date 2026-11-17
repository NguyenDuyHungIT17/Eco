import { Button, type ButtonProps } from 'antd';
import { useAbility } from '@casl/react';
import { AbilityContext } from 'shared/services/CaslService';

type PermButtonProps = ButtonProps & {
  // Một quyền, hoặc danh sách quyền (có 1 trong số đó là được)
  perm?: string | string[];
};

// Nút tự ẩn khi người dùng không có quyền — giống prop `perm` của các nút trong Frontends
export function PermButton({ perm, ...props }: PermButtonProps) {
  const ability = useAbility(AbilityContext);
  const perms = perm ? (Array.isArray(perm) ? perm : [perm]) : [];

  if (perms.length > 0 && !perms.some((p) => ability.can(p, 'all'))) {
    return null;
  }
  return <Button {...props} />;
}
