import { Navigate, useLocation } from "react-router-dom";
import { isExpired } from "react-jwt"

import { NAVIGATION_PATH } from "@/constants";
import Loader from "../Loader";
import { Suspense } from "react";
import useAppSelector from "@/hooks/useAppSelector";
import { UserProfile } from "@/types/api/enums/UserProfile";

interface AuthGuardType {
  belongsTo?: readonly UserProfile[]
  children: React.ReactNode;
}

function AuthGuard({ children, belongsTo }: AuthGuardType) {
  const { access_token, user } = useAppSelector(state => state.auth);
  const { pathname } = useLocation();

  if ((access_token == null || isExpired(access_token))) {
    let route = `${NAVIGATION_PATH.AUTH.SIGN_IN.ABSOLUTE}`;
    if (pathname !== '/') {
      const searchParams = new URLSearchParams({ redirect_uri: pathname });
      route += `?${searchParams}`;
    }

    return <Navigate to={route} />
  }

  if (belongsTo?.length && !belongsTo.includes(user?.profile as UserProfile)) {
    return <Navigate to={NAVIGATION_PATH.DASHBOARD.ROOT} replace />;
  }

  return (
    <Suspense fallback={<Loader />}>
      {children}
    </Suspense>
  );
}

export default AuthGuard;
