import { Outlet, Navigate } from 'react-router-dom'
import { Utility } from '../utilities/utility';
import { LinkConstants } from '../entities/constants';
import { useAppTrackedSelector } from './store';

export const PrivateRoute = () => {
    const appData = useAppTrackedSelector();
    let isUserLoggedIn = Utility.isUserLoggedIn(appData);
    return isUserLoggedIn === true ? <Outlet /> : <Navigate to={Utility.getLink(LinkConstants.LOGIN)} />;
}