import { Route, Routes } from 'react-router-dom'
import { GlobalStateProvider, appStore, appPersistor, AppSlice } from './utilities/store';
import './App.css';
import { NavBar } from './components/shared/navbar'
import { Home } from "./components/home";
import { Settings } from "./components/settings";
import { PrivateRoute } from './utilities/privateroute';
import { Utility } from './utilities/utility';
import { LinkConstants } from './entities/constants';
import { Confirm } from './components/shared/confirm';
import { Message } from './components/shared/message';
import { Loader, LoaderStart } from './components/shared/loader';
import { Provider } from 'react-redux';
import { Login } from './components/login';
import { PersistGate } from 'redux-persist/integration/react'
import { FetchData } from './components/FetchData';

const OnBeforeLift = async () => {
    const { setAppModel } = AppSlice.actions;
    await Utility.getAppData().then(appData => {
        appStore.dispatch(setAppModel(appData));
    });
}

const App = () => {
    return (
        <Provider store={appStore}>
            <PersistGate loading={<LoaderStart />} persistor={appPersistor} onBeforeLift={ async () => await OnBeforeLift()}>
                <GlobalStateProvider  >
                    <div className="App">
                        <NavBar />
                        <Routes>
                            <Route path={Utility.getLink(LinkConstants.LOGIN)} element={<Login />} />
                            <Route path={Utility.getLink(LinkConstants.HOME)} element={<PrivateRoute />}>
                                <Route path={Utility.getLink(LinkConstants.HOME)} element={<Home />} />
                                <Route path={Utility.getLink(LinkConstants.SETTINGS)} element={<Settings />} />
                                <Route path={Utility.getLink(LinkConstants.FETCHDATA)} element={<FetchData />} />
                            </Route>
                        </Routes>
                    </div>
                    <Confirm></Confirm>
                    <Message></Message>
                    <Loader></Loader>
                </GlobalStateProvider>
            </PersistGate>
        </Provider>
    )
}

export default App;