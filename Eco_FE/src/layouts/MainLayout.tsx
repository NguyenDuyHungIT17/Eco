import Dashboard from 'modules/home'
import Login from 'modules/auth'

export default function MainLayout() {
  return <><header className="header"><a className="brand" href="/">Eco.</a><span>NỀN TẢNG QUẢN LÝ</span></header><main className="container"><div className="auth-shell"><Dashboard /><Login /></div></main></>
}
