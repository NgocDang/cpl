Use CPL;

ALTER TABLE SysUser
  ADD TwoFactorAuthenticationEnable bit not null DEFAULT (0);