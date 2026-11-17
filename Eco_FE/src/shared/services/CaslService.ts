import { createContext } from 'react';
import { AbilityBuilder, createMongoAbility, type MongoAbility } from '@casl/ability';
import { createContextualCan } from '@casl/react';
import { store } from 'store/store';

// Quyền lấy từ claim `permission` trong JWT, ví dụ "admin.nguoi-dung.xem".
// Dùng: ability.can('admin.nguoi-dung.xem', 'all') hoặc <Can I="..." a="all">
export const ability = createMongoAbility();

let lastPermissions: string[] | null = null;

function updateAbility() {
  const permissions = store.getState().oauth.permissions;
  if (permissions === lastPermissions) {
    return;
  }
  lastPermissions = permissions;

  const { can, rules } = new AbilityBuilder<MongoAbility>(createMongoAbility);
  permissions.forEach((permission) => can(permission, 'all'));
  ability.update(rules);
}

updateAbility();
store.subscribe(updateAbility);

export const AbilityContext = createContext<MongoAbility>(ability);
export const Can = createContextualCan(AbilityContext.Consumer);
