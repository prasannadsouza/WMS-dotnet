import { Outlet, Navigate, useLocation, useNavigate } from 'react-router-dom'
import { Utility } from '../utilities/utility';
import { LinkConstants } from '../entities/constants';
import { useAppTrackedSelector } from './store';
import { useEffect } from 'react';


export const PrivateRoute = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const appData = useAppTrackedSelector();
    let isUserLoggedIn = Utility.isUserLoggedIn(appData);

    useEffect(() => {
        if (appData?.sessionData?.isAuthenticated !== true) {
            navigate(Utility.getLink(LinkConstants.LOGIN));
        }
    }, [appData?.sessionData?.isAuthenticated]);

    return isUserLoggedIn === true ? <Outlet /> : <Navigate to={Utility.getLink(LinkConstants.LOGIN)} state={{ from: location }} />;
}