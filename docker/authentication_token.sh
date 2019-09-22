#!/bin/bash

#wait for root user is added to table
root_available=false
while [ "$root_available" = false ]
do
    if [ $(gitlab-psql -d gitlabhq_production -c "SELECT * from users WHERE username='root'" | wc -l) -eq 5 ]; then
        root_available=true
        echo "root found in user table"
    else
        echo "root doesn't exists in user table"
        sleep 15
    fi
done

#update-permissions
updated=false
while [ "$updated" = false ]
do
    gitlab-rails runner "User.admins.first.personal_access_tokens.create(name: 'testtoken', token_digest: Gitlab::CryptoHelper.sha256('ElijahBaley'), impersonation: false, scopes: [:api, :read_user, :read_repository, :write_repository, :sudo])" >/dev/null
    if [ $? -eq 0 ]; then
        updated=true
        echo "Success updated root authentication_token"
    else
        echo "Unable to update root authentication_token retry in 30s"
        sleep 15
    fi
done