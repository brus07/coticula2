#!/bin/bash

apiUrl="https://ci.appveyor.com/api"
accountName="brus07"
projectSlug="coticula2"
buildBranch="develop"
destinationFolder="."

echo "Get jobId from '${apiUrl}/projects/${accountName}/${projectSlug}/branch/${buildBranch}'"
# curl -s -H "Content-Type:application/json" https://ci.appveyor.com/api/projects/brus07/coticula2/branch/develop
jobId="$(curl -s -H "Content-Type:application/json" ${apiUrl}/projects/${accountName}/${projectSlug}/branch/${buildBranch} | jq '.build.jobs[0].jobId' | awk '{print $1}' | sed 's/\"//g')"
jobStatus="$(curl -s -H "Content-Type:application/json" ${apiUrl}/projects/${accountName}/${projectSlug}/branch/${buildBranch} | jq '.build.jobs[0].status' | awk '{print $1}' | sed 's/\"//g')"
echo "jobId=${jobId}; Status=${jobStatus}"

if [[ "${jobStatus}" !=  "success" ]]; then
	echo "Last job hasn't Success status. The status is '${jobStatus}'."
	exit
fi

# continue only if jobId is updated
lastjobidfile="lastjobid.txt"
if [ ! -z "$1" ]; then
	rm "${lastjobidfile}"
	echo "Force update."
fi
if [ -f "${lastjobidfile}" ]; then
	if [[ "$(cat ${lastjobidfile})" ==  "${jobId}" ]]; then
		echo "There is no new version availbale at this time."
		exit
	fi
fi

echo "There is NEW version."
echo "Start updating procedure."

echo "Get artifact name from '${apiUrl}/buildjobs/${jobId}/artifacts'"
artifactFileName="$(curl -s -H "Content-Type:application/json" ${apiUrl}/buildjobs/${jobId}/artifacts | jq '.[0].fileName' | awk '{print $1}' | sed 's/\"//g')"

fullArtifactUrl="${apiUrl}/buildjobs/${jobId}/artifacts/${artifactFileName}"
echo "Full artifact URL is '${fullArtifactUrl}'"

buildNumber="$(curl -s -H "Content-Type:application/json" ${apiUrl}/projects/${accountName}/${projectSlug}/branch/${buildBranch} | jq '.build.buildNumber')"
newArtifactFileName="${artifactFileName/./.${buildNumber}.}"

echo "Downloading artifact zip file..."
wget -q "${fullArtifactUrl}" -O "${newArtifactFileName}"
echo "File '${newArtifactFileName}' was downloaded."

# Unzip and copy to destination files
echo "Unzipping..."
unzip -qo "${newArtifactFileName}" -d tempunzip

echo "Updating files..."
mkdir -p "${destinationFolder}"/F
mkdir -p "${destinationFolder}"/F/S
mkdir -p "${destinationFolder}"/F/S/T
cp tempunzip/Client/bin/Debug/* "${destinationFolder}"/F/S/T/
mkdir -p "${destinationFolder}"/Tools
mkdir -p "${destinationFolder}"/Tools/timeout
cp tempunzip/Tools/timeout/* "${destinationFolder}"/Tools/timeout/

chmod +x "${destinationFolder}"/F/S/T/Coticula2.Face.APIClient.exe
chmod +x "${destinationFolder}"/Tools/timeout/timeout

cp Coticula2.Face.APIClient.exe.config "${destinationFolder}"/F/S/T/

rm -r tempunzip

# save job id for success deployment
echo "${jobId}" > "${lastjobidfile}"

echo "The build '${buildNumber}' was successfully deployed."