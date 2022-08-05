import { Outlet, Navigate } from 'react-router-dom'
import { Utility } from '../utilities/utility';
import { LinkConstants } from '../entities/constants';
import { useAppTrackedSelector } from './store';

export const PrivateRoute = () => {
    const appData = useAppTrackedSelector();
    const appConfig = appData.appConfig;

    let isUserLoggedIn = Utility.isUserLoggedIn(appConfig);
    return isUserLoggedIn === true ? <Outlet /> : <Navigate to={Utility.getLink(LinkConstants.LOGIN)} />;
}