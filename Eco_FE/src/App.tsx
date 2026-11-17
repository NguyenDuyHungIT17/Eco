import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { RouterProvider } from 'react-router-dom';
import { App as AntApp, ConfigProvider } from 'antd';
import viVN from 'antd/locale/vi_VN';
import dayjs from 'dayjs';
import 'dayjs/locale/vi';
import { store, persistor } from 'store/store';
import { AbilityContext, ability } from 'shared/services';
import { FallbackElement } from 'shared/components';
import { router } from './AppRoutes';

dayjs.locale('vi');

const theme = {
  token: {
    colorPrimary: '#176b4b',
    fontFamily: '"Segoe UI", "Aptos", sans-serif',
  },
};

export default function App() {
  return (
    <Provider store={store}>
      {/* Chờ redux-persist đọc token từ localStorage xong mới render → F5 không mất đăng nhập */}
      <PersistGate loading={<FallbackElement />} persistor={persistor}>
        <ConfigProvider locale={viVN} theme={theme}>
          <AntApp>
            <AbilityContext.Provider value={ability}>
              <RouterProvider router={router} fallbackElement={<FallbackElement />} />
            </AbilityContext.Provider>
          </AntApp>
        </ConfigProvider>
      </PersistGate>
    </Provider>
  );
}
