# Static Rates

An MVC app and API service that implements the Static Rate Service portion of Static Rates architecture. 
Architecture [in this confluence](http://confluence.ozforex.local/display/PRIC/Static+Rates)

## Prerequisites
* .netcore 2.2

## Run Locally
1. Open solution in Visual Studio
1. **Important** Switch the Startup project from `IISExpress` to `Docker`
1. Hit F5.
1. The Service communicates with docker running on port 60000. If there is already a static rates service running on this port by default, stop the default static rates container from running by using `docker stop {container id}`

## Important Docker Info
* The `docker-compose.override.yml` can be changed locally to set up docker to run in any way you like
* Any local changes made should not be commited into the repo as the changes can break Docker for other users
* To stop git tracking this file run the following command "`git update-index --assume-unchanged docker-compose/docker-compose.override.yml`"
* To start git tracking this again run the following command "`git update-index --no-assume-unchanged docker-compose/docker-compose.override.yml`"

## Deployment
Follow [this ID server setup document in confluence which will be the same for static rates](http://confluence.ozforex.local/display/ISV/Deploying+Identity+Server+into+a+new+Environment)

### Troubleshooting

Static rates uses logging.
Therefore, you can use [ELK locally](http://confluence.ozforex.local/display/BSL/Running+a+local+ELK+stack)
 to read the exceptions

# How do deploy using Pipeline (not used at the moment)

## WARNING

### Make sure you always use LF line endings with .sh files, otherwise the scripts will fail.

## Deployment

After a successful deployment you'll be able to access the website via the following DNS `web.<feature>.<build#>.srdt.non.c1.ofx.com`. Alternatively, you can navigate to the logs of the Bamboo CD and get the `web.BuildDNS` value.

## Release

After a release the pipeline change the DNS to `web.<feature>.srdt.non.c1.ofx.com`.

The difference between them is only the build number and internally the Release DNS is just a CNAME to the selected build.

## Teardown

The teardown step will remove all resources created by pipeline.

## Workflow

How to keep the website workflow in 10 steps

1. Commit code to branch feature/PEN-1234
2. Deploy build #1
    * New website created on `web.feature-pen-1234.b1.srdt.non.c1.ofx.com`
3. Release build #1
    * New DNS `web.feature-pen-1234.srdt.non.c1.ofx.com` pointing to `web.feature-pen-1234.b1.srdt.non.c1.ofx.com`
4. New commit with extra code
5. Deploy build #2
    * New website created on `web.feature-pen-1234.b2.srdt.non.c1.ofx.com`
6. Changes not good
    * Need to teardown the new environment without changing the release
    * On Bamboo CD you'll run a custom build selecting the `Teardown` stage and overriding a build variable `skip_release` with `true`
    * No changes are made to `web.feature-pen-1234.b1.srdt.non.c1.ofx.com`
7. New commit with code fix
8. Deploy build #3
    * New website created on `web.feature-pen-1234.b3.srdt.non.c1.ofx.com`
9. Release build #3
    * New DNS `web.feature-pen-1234.srdt.non.c1.ofx.com` pointing to `web.feature-pen-1234.b3.srdt.non.c1.ofx.com`
10. Go back to build #1 and execute `Teardown`
